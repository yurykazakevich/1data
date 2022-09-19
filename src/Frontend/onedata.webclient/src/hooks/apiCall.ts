import axios, { AxiosError, AxiosResponse } from 'axios'
import { useRef, useContext } from 'react'
import { LoaderContext } from '../context/LoaderContext'

interface IApiCallResponse<TResponse> {
    response: TResponse | any,
    apiError: string
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

    function buildApiUrl(relativeUrl: string): string {
        var baseApiUrl = process.env.REACT_APP_API_URL
        if (!baseApiUrl?.endsWith('/')) {
            baseApiUrl += '/'
        }

        return baseApiUrl + relativeUrl;
    }

    async function makeRequest(data: TRequest): Promise<IApiCallResponse<TResponse>> {

       var response: IApiCallResponse<TResponse> = { apiError: '', response: null}
        var axiosResponse: AxiosResponse<TResponse, any>
        const apiUrl = buildApiUrl(url)

        try {
            showLoader()

            switch (method) {
                case ApiMethods.GET:
                    axiosResponse = await axios.get<TResponse>(apiUrl)
                    response.response = axiosResponse.data as TResponse
                    break
                case ApiMethods.POST:
                    axiosResponse = await axios.post<TResponse>(apiUrl, data)
                    response.response = axiosResponse.data as TResponse
                    break
                default:
                    var methodStr: string = ApiMethods[method];
                    console.error('Unspported API method: ' + methodStr)
            }
        } catch (e: unknown) {
            const error = e as AxiosError
            response.apiError = error.message
        } finally {
            hideLoader()
        }

        return response
    }

    return { makeRequest }
}