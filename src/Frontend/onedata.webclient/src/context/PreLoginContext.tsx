import { createContext } from 'react'

interface IPreLoginContext {
    phoneNumber: string
    verificationCode: string,
}

export const PreLoginContext = createContext<IPreLoginContext>({
    phoneNumber: '',
    verificationCode: '',
})

