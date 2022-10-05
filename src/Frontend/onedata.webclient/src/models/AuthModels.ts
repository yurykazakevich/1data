export interface IPhoneNumberRequest {
    phoneNumber: string
}

export interface IVerificationCodeResponse {
    code: string
}

export interface ILoginReuest extends IPhoneNumberRequest {
    verificationCode: string
    userProvidedCode: string
    isPhisical: boolean
}

export interface ITokenResponse {
    token: string
    tokenExpired: Date,
    userId: number,
    phoneNumber: string
}

export interface IRefreshTokenRequest extends IPhoneNumberRequest {
    expiredToken: string
}