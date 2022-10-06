import { createContext } from 'react'

interface IPreLoginContext {
    phoneNumber: string
    isOrg: boolean,
    verificationCode: string,
}

export const PreLoginContext = createContext<IPreLoginContext>({
    phoneNumber: '',
    isOrg: false,
    verificationCode: '',
})

