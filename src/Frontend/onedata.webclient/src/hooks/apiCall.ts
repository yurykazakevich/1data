import axios, { AxiosError, AxiosRequestConfig, AxiosResponse } from 'axios'
import { useContext } from 'react'
import { JwtContext } from '../context/JwtContext'
import { LoaderContext } from '../context/LoaderContext'
import { ModalContext } from '../context/ModalContext'
import { IValidationErrorResponse, IErrorResponse } from '../models/ErrorModels'
import { IRefreshTokenRequest, ITokenResponse } from '../models/AuthModels'


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

export function useApiCall<TRequest, TResponse>(url: string, method: ApiMethods) {

    const { showLoader, hideLoader } = useContext(LoaderContext)
    //const { modal, open, close } = useContext(ModalContext)
    const jwtContext = useContext(JwtContext)

    function buildApiUrl(relativeUrl: string): string {
        var baseApiUrl = process.env.REACT_APP_API_URL
        if (!baseApiUrl?.endsWith('/')) {
            baseApiUrl += '/'
        }

        return baseApiUrl + relativeUrl;
    }

    async function VerifyJwtToken(): Promise<string> {
        if (jwtContext.isTokenExpired(jwtContext.data)) {
            await RefreshToken()
        }

        return jwtContext.data.jwtToken
    }

    async function RefreshToken(): Promise<void> {
        var jwtResponse: ITokenResponse = await MakeRefreshTokenRequest()

        jwtContext.setFromResponse(jwtContext.data, jwtResponse)
    }

    async function MakeRefreshTokenRequest(): Promise<ITokenResponse> {
        var requestConfig: AxiosRequestConfig = {
            headers: {
                'Content-Type': 'application/json',
            }
        }

        var axiosResponse: AxiosResponse<TResponse, any>
        const apiUrl = buildApiUrl("auth/token")

        var request: IRefreshTokenRequest = {
            expiredToken: jwtContext.data.jwtToken,
            phoneNumber: jwtContext.data.phoneNumber,
            refreshToken: jwtContext.data.refreshToken
        } 

        axiosResponse = await axios.patch<TResponse>(apiUrl, request, requestConfig)
        return axiosResponse.data as ITokenResponse
    }

    function chackAuthorize(mustBeAuthorized: boolean,
        requestConfig: AxiosRequestConfig,
        response: IApiCallResponse<TResponse>) {
        if (mustBeAuthorized) {
            response.apiError = isLoggedIn()
            if (response.apiError !== null) {
                return
            }

            requestConfig.headers = {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + VerifyJwtToken()
            }
        }
        else {
            requestConfig.headers = {
                'Content-Type': 'application/json',
            }
        }
    }
    function isLoggedIn(): IErrorResponse  | null {
        if (jwtContext.isAuthorized(jwtContext.data)) {
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



    async function makeRequest(data: TRequest, authorize: boolean = true): Promise<IApiCallResponse<TResponse>> {

        var response: IApiCallResponse<TResponse> = { apiError: '', response: null }
        var requestConfig: AxiosRequestConfig = {
            headers: {}
        }

        var axiosResponse: AxiosResponse<TResponse, any>
        const apiUrl = buildApiUrl(url)

        try {
            showLoader()

            chackAuthorize(authorize, requestConfig, response)
            if (response.apiError !== null) {
                return response
            }

            switch (method) {
                case ApiMethods.GET:
                    axiosResponse = await axios.get<TResponse>(apiUrl, requestConfig)
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
            if (error.code === 'ERR_BAD_REQUEST'
                && (error.response?.data as IValidationErrorResponse) !== null) {
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