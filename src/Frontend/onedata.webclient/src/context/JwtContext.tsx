import { useState, createContext } from 'react'
import { ITokenResponse } from '../models/AuthModels'


export interface IJwtContextData {
    tokenCreated: Date
    jwtToken: string
    jwtTokenExpired: number,
    refreshToken: string,
    userId: number
    phoneNumber: string
}

export interface IJwtContext {
    data: IJwtContextData,
    isTokenExpired(data: IJwtContextData): boolean,
    isAuthorized(data: IJwtContextData): boolean,
    reset(data: IJwtContextData): void,
    setFromResponse(data: IJwtContextData, jwtResponse: ITokenResponse): void
}

export const JwtContext = createContext<IJwtContext>({
    data: {
        tokenCreated: new Date(),
        jwtToken: '',
        jwtTokenExpired: 0,
        refreshToken: '',
        userId: 0,
        phoneNumber: ''
    },
    isTokenExpired: (data: IJwtContextData) => {
        return true
    },
    isAuthorized: (context: IJwtContextData) => {
        return false
    },
    reset: (data: IJwtContextData) => { },
    setFromResponse: (data: IJwtContextData, jwtResponse: ITokenResponse) => {}
})

export const JwtState = ({ children }: { children: React.ReactNode }) => {
    const data: IJwtContextData = {
        tokenCreated: new Date(),
        jwtToken: '',
        jwtTokenExpired: 0,
        refreshToken: '',
        userId: 0,
        phoneNumber: ''
    }
    const isTokenExpired = (data: IJwtContextData) => {
            var now = new Date()
            return (+now - +data.tokenCreated) * 60000 > data.jwtTokenExpired
        }
    const isAuthorized = (data: IJwtContextData) => {
            return data.jwtToken.length > 0 && data.refreshToken.length > 0 && data.userId > 0
        }
    const reset = (data: IJwtContextData) => {
        data.tokenCreated = new Date()
        data.jwtToken = ''
        data.jwtTokenExpired = 0
        data.refreshToken = ''
        data.userId = 0
        data.phoneNumber = ''
    }
    const setFromResponse = (data: IJwtContextData, jwtResponse: ITokenResponse) {
        data.jwtToken = jwtResponse.token
        data.jwtTokenExpired = jwtResponse.tokenExpired
        data.refreshToken = jwtResponse.refreshToken
        data.tokenCreated = new Date(jwtResponse.tokenCreated)
        data.userId = jwtResponse.userId
        data.phoneNumber = jwtResponse.phoneNumber
    }

    return (
        <JwtContext.Provider value={{ data, isTokenExpired, isAuthorized, reset, setFromResponse }}>
            {children}
        </JwtContext.Provider>
    )
}
