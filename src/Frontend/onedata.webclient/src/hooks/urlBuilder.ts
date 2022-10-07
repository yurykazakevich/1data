export function useUrlBuilder() {
    const baseApiUrl = process.env.REACT_APP_API_URL

    function buildApiUrl(relativeUrl: string): string {

        var url = baseApiUrl

        if (!baseApiUrl?.endsWith('/')) {
            url += '/'
        }

        return url + relativeUrl;
    }

    return { buildApiUrl }
}