export interface IPhoneNumberRequest {
    phoneNumber: string,
    isPhisical: boolean
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
    phoneNumber: string,
    isPhisical: boolean
}

export interface IRefreshTokenRequest extends IPhoneNumberRequest {
    expiredToken: string
}