export interface IPhoneNumberRequest {
    phoneNumber: string
}

export interface IVerificationCodeResponse {
    code: string
}

export interface ILoginReuest extends IPhoneNumberRequest {
    verificationCode: string
    userProvidedCode: string
}

export interface ITokenResponse {
    token: string
    refreshToken: string
    tokenExpired: number,
    tokenCreated: Date,
    userId: number,
    phoneNumber: string
}

export interface IRefreshTokenRequest {
    phoneNumber: string
    expiredToken: string
    refreshToken: string
}