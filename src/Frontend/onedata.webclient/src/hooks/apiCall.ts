import axios, { AxiosError, AxiosResponse } from 'axios'
import { useState } from 'react'

enum ApiMethods {
    GET,
    POST,
    PUT,
    PATCH,
    DELETE
}

export function useApiCall<TResponse>(url: string, method: ApiMethods, data: any) {

    const [response, setResponse] = useState<TResponse>()
    const [apiError, setApiError] = useState('')

    function buildApiUrl(relativeUrl: string): string {
        //TODO: Add building from config
        return relativeUrl;
    }

    async function makeRequest(url: string, method: ApiMethods, data: any, setLoading: (visible: boolean) => {}) {

        var axiosResponse: AxiosResponse<TResponse, any>
        const apiUrl = buildApiUrl(url)

        try {
            setApiError('')
            setLoading(true)

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
            setLoading(false)
            setApiError(error.message)
        } finally {
            setLoading(false)
        }
    }

    return { response, apiError, makeRequest }
}