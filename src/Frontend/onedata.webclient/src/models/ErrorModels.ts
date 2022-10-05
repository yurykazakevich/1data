export interface IErrorResponse {
    statusCode: number,
    errorMessage: string
}

interface IValidationErrorResponseItem {
    propertyName: string,
    message: string
}

export interface IValidationErrorResponse {
    validationErrors: Array<IValidationErrorResponseItem>
}