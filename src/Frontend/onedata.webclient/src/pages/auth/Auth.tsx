import { Outlet } from 'react-router-dom'
import { PreLoginContext } from '../../context/PreLoginContext';

export function Auth() {
    const phoneNumber: string = ''
    const verificationCode: string = ''

    return (
        <>
            <PreLoginContext.Provider value={{ phoneNumber, verificationCode }}>
                <h1>Авторизация</h1>
                <Outlet />
             </PreLoginContext.Provider>
        </>
    );
}
