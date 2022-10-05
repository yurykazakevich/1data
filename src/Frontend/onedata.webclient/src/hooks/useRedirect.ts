import { useNavigate } from 'react-router-dom'

export function useRedirect() {
    const loginUrl = process.env.REACT_APP_LOGIN_URL ?? '/auth'
    const homeUrl = process.env.REACT_APP_HOME_URL ?? '/'
    const monumentBuilderUrl = process.env.REACT_APP_MONUMENTBUILDER_URL ?? '/monumentBuilder'
    const navigate = useNavigate()

    function redirectToLogin() {
        navigate(loginUrl)
    }

    function redirectToHome() {
        navigate(homeUrl)
    }

    function redirectToMonumentBuilder() {
        navigate(monumentBuilderUrl)
    }

    function redirectToPage(pageUrl: string) {
        navigate(pageUrl)
    }

    return { redirectToLogin, redirectToHome, redirectToMonumentBuilder, redirectToPage }
}