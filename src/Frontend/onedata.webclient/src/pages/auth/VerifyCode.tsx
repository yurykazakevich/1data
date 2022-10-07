import React, { useState, useEffect, useContext } from 'react'
import { useApiCall, ApiMethods } from '../../hooks/apiCall'
import { ILoginReuest, ITokenResponse } from '../../models/AuthModels'
import { PreLoginContext } from '../../context/PreLoginContext'
import { IValidationErrorResponse } from '../../models/ErrorModels'
import { useRedirect } from '../../hooks/redirect'
import { Button, Form, InputGroup } from 'react-bootstrap'
import { useJwtData } from '../../hooks/jwtData'

export function VerifyCode() {
    const codeLength = parseInt(process.env.REACT_APP_VERIFICATION_CODE_LENGTH as string)
    const defaultCodeErrorMessage = 'Код должен состоять из ' + codeLength + '-ти цифр'
    const [codeError, setCodeError] = useState(defaultCodeErrorMessage)
    const [code, setCode] = useState('')
    const [isCodeValid, setIsCodeValid] = useState(true)
    const sendSmsCall = useApiCall<ILoginReuest, ITokenResponse>("auth/login", ApiMethods.POST)
    const preLoginContext = useContext(PreLoginContext)
    const redirect = useRedirect()
    const jwtData = useJwtData()

    useEffect(() => {
        if (!preLoginContext.phoneNumber || preLoginContext.phoneNumber.length === 0 ||
            !preLoginContext.verificationCode || preLoginContext.verificationCode.length === 0) {
            redirect.redirectToLogin()
        }
    }, [])

    const submitHandler = async (event: React.FormEvent) => {
        event.preventDefault()
        setCodeError(defaultCodeErrorMessage)
        setIsCodeValid(true)

        var codeLength = parseInt(process.env.REACT_APP_VERIFICATION_CODE_LENGTH as string)

        if (code.trim().length !== codeLength) {
            setIsCodeValid(false)
            return
        }

        const request: ILoginReuest = {
            phoneNumber: preLoginContext.phoneNumber,
            userProvidedCode: code,
            verificationCode: preLoginContext.verificationCode,
            isPhisical: !preLoginContext.isOrg
        }

        const response = (await sendSmsCall.makeRequest(request, false))

        if (response.response !== null) {
            const jwtResponse = response.response

            jwtData.setData(jwtResponse)

            preLoginContext.phoneNumber = ''
            preLoginContext.verificationCode = ''
            preLoginContext.isOrg = false

            redirect.redirectToMonumentBuilder()
        }
        else {
            if (response.apiError.validationErrors) {
                const validatioErrors = response.apiError as IValidationErrorResponse
                var isFormField: boolean = false
                for (var i = 0; i < validatioErrors.validationErrors.length; i++) {
                    if (validatioErrors.validationErrors[i].propertyName === 'userProvidedCode') {
                        isFormField = true
                        setCodeError(validatioErrors.validationErrors[i].message)
                        setIsCodeValid(true)
                    }
                }

                if (!isFormField) {
                    alert("Ошибка валидации на сервере")
                }
            }
            else {
                alert(response.apiError)
            }
        }
    }

    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setCode(event.target.value)
    }

    return (
        <>
            <div className="text-center">
                <h2>Введите код</h2>
                <p>Введите код, который Вы получили в СМС. После этого Вы сможете начать оформление заказа</p>
            </div>
            <InputGroup className="mb-3" hasValidation>
                <InputGroup.Text className="custom-item-group">Ваш код:</InputGroup.Text>
                <Form.Control
                    isInvalid={!isCodeValid}
                    onChange={handleChange}
                    value={code} />
                <Form.Control.Feedback type="invalid">
                    { codeError }
                </Form.Control.Feedback>
            </InputGroup>
            <div className="d-grid gap-2">
                <Button variant="dark" type="button" size="lg" onClick={submitHandler}>
                    Войти
                </Button>
            </div>
        </>
    )
}