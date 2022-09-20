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
}