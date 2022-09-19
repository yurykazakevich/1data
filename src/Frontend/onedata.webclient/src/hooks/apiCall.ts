import axios, { AxiosError, AxiosResponse } from 'axios'
import { useState, useContext } from 'react'
import { LoaderContext } from '../context/LoaderContext'

interface IApiCallResponse<TResponse> {
    response: TResponse,
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

    const [response, setResponse] = useState<TResponse>()
    const [apiError, setApiError] = useState('')
    const { showLoader, hideLoader } = useContext(LoaderContext)

    function buildApiUrl(relativeUrl: string): string {
        var baseApiUrl = process.env.REACT_APP_API_URL
        if (!baseApiUrl?.endsWith('/')) {
            baseApiUrl += '/'
        }

        return baseApiUrl + relativeUrl;
    }

    async function makeRequest(data: TRequest): Promise<IApiCallResponse<TResponse>> {

        var axiosResponse: AxiosResponse<TResponse, any>
        const apiUrl = buildApiUrl(url)

        try {
            setApiError('')
            showLoader()

            switch (method) {
                case ApiMethods.GET:
                    axiosResponse = await axios.get<TResponse>(apiUrl)
                    setResponse(axiosResponse.data)
                    break
                case ApiMethods.POST:
                    axiosResponse = await axios.post<TResponse>(apiUrl, data)
                    setResponse(axiosResponse.data)
                    break
                default:
                    var methodStr: string = ApiMethods[method];
                    console.error('Unspported API method: ' + methodStr)
            }
        } catch (e: unknown) {
            const error = e as AxiosError
            setApiError(error.message)
        } finally {
            hideLoader()
        }

        return { response, apiError } as IApiCallResponse<TResponse>
    }

    return { makeRequest }
}