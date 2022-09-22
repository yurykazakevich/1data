import { useNavigate } from 'react-router-dom'

export function useRedirect() {
    const loginUrl = process.env.REACT_APP_LOGIN_URL ?? 'auth'
    const homeUrl = process.env.REACT_APP_HOME_URL ?? '/'
    const navigate = useNavigate()

    function redirectToLogin() {
        navigate(loginUrl)
    }

    function redirectToHome() {
        navigate(homeUrl)
    }

    function redirectToPage(pageUrl: string) {
        navigate(pageUrl)
    }

    return { redirectToLogin, redirectToHome, redirectToPage }
}