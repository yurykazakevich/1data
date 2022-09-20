export interface IErrorResponse {
    statusCode: number,
    errorMessage: string
}

interface IValidationErrorResponseItem {
    propertyName: string,
    message: string
}

export interface IValidationErrorResponse {
    errors: Array<IValidationErrorResponseItem>
}