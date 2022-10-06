import axios, { AxiosError, AxiosRequestConfig, AxiosResponse } from 'axios'
import { useContext } from 'react'
import { LoaderContext } from '../context/LoaderContext'
import { ModalContext } from '../context/ModalContext'
import { IValidationErrorResponse, IErrorResponse } from '../models/ErrorModels'
import { IRefreshTokenRequest, ITokenResponse } from '../models/AuthModels'
import { GlobalStrings } from '../models/Values'
import { useJwtData } from './jwtData'


interface IApiCallResponse<TResponse> {
    response: TResponse | null,
    apiError: any
}

export enum ApiMethods {
    GET,
    POST,
    PUT,
    PATCH,
    DELETE
}

export function useApiCall<TRequest extends {} , TResponse>(url: string, method: ApiMethods) {

    const jwtData = useJwtData()
    const { showLoader, hideLoader } = useContext(LoaderContext)
    //const { modal, open, close } = useContext(ModalContext)

    function buildApiUrl(relativeUrl: string): string {
        var baseApiUrl = process.env.REACT_APP_API_URL
        if (!baseApiUrl?.endsWith('/')) {
            baseApiUrl += '/'
        }

        return baseApiUrl + relativeUrl;
    }

    async function VerifyJwtToken(): Promise<string | undefined> {
        var jwt = jwtData.getData()
        if (jwt !== null) {
            var now = new Date()
            if (now > new Date(jwt.tokenExpired)) {
                jwt = await RefreshToken(jwt.phoneNumber, jwt.isPhisical, jwt.token)
                jwtData.setData(jwt)
                localStorage.setItem(GlobalStrings.jwtDataKey, JSON.stringify(jwtData))
            }
        }

        return jwt?.token
    }

    async function RefreshToken(phoneNumber: string, isPhisical: boolean, token: string): Promise<ITokenResponse> {
        var jwtResponse: ITokenResponse = await MakeRefreshTokenRequest(phoneNumber, isPhisical, token)

        return jwtResponse;
    }

    async function MakeRefreshTokenRequest(phoneNumber: string, isPhisical: boolean, token: string): Promise<ITokenResponse> {
        var requestConfig: AxiosRequestConfig = {
            headers: {
                'Content-Type': 'application/json',
            }
        }

        var axiosResponse: AxiosResponse<TResponse, any>
        const apiUrl = buildApiUrl("auth/token")

        var request: IRefreshTokenRequest = {
            expiredToken: token,
            phoneNumber: phoneNumber,
            isPhisical: isPhisical
        } 

        axiosResponse = await axios.patch<TResponse>(apiUrl, request, requestConfig)
        return axiosResponse.data as ITokenResponse
    }

    async function checkAuthorize(mustBeAuthorized: boolean,
        requestConfig: AxiosRequestConfig,
        response: IApiCallResponse<TResponse>) {
        if (mustBeAuthorized) {
            response.apiError = isLoggedIn()
            if (response.apiError !== null) {
                return
            }

            requestConfig.headers = {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + (await VerifyJwtToken())
            }
        }
        else {
            requestConfig.headers = {
                'Content-Type': 'application/json',
            }
        }
    }

    function isLoggedIn(): IErrorResponse | null {
        var jwt = jwtData.getData()
        if (!jwt || !jwt.token) {
            console.debug("Попытка выполнить запрос " + url + " без авторизации")

            var errorResponse: IErrorResponse = {
                statusCode: 401,
                errorMessage: "Пользователь не авторизован"
            }

            return errorResponse
        }
        else {
            return null
        }
    }

    function ToQueryString(data: TRequest): string {
        var keys = Object.keys(data)
        var values = Object.values(data)
        var queryString = '?'

        for (var i = 0; i < keys.length; i++) {
            queryString += keys[i] + '=' + values[i]
            if (i < keys.length - 1) {
                queryString += '&'
            }
        }

        return queryString
    }

    async function makeRequest(data: TRequest, authorize : boolean = true): Promise<IApiCallResponse<TResponse>> {

        var response: IApiCallResponse<TResponse> = { apiError: null, response: null }
        var requestConfig: AxiosRequestConfig = {
            headers: {},
            withCredentials: true,
        }

        if (url.startsWith("image")) {
            requestConfig.responseType = "blob"
        }

        var axiosResponse: AxiosResponse<TResponse, any>
        const apiUrl = buildApiUrl(url)

        try {
            showLoader()

            await checkAuthorize(authorize, requestConfig, response)
            if (response.apiError !== null) {
                return response
            }

            switch (method) {
                case ApiMethods.GET:
                    var queryString = ToQueryString(data)
                    axiosResponse = await axios.get<TResponse>(apiUrl + queryString, requestConfig)
                    response.response = axiosResponse.data as TResponse
                    break
                case ApiMethods.POST:
                    axiosResponse = await axios.post<TResponse>(apiUrl, data, requestConfig)
                    response.response = axiosResponse.data as TResponse
                    break
                default:
                    var methodStr: string = ApiMethods[method];
                    console.error('Unspported API method: ' + methodStr)
            }
        } catch (e: unknown) {
            const error = e as AxiosError
            console.error(error.message)
            if (error.code === 'ERR_BAD_REQUEST') {
                response.apiError = error.response?.data
            }
            else {
                response.apiError = error.message
            }
        } finally {
            hideLoader()
        }

        return response
    }

    return { makeRequest }
}