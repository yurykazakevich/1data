import React, { useState, useContext, useEffect } from 'react'
import { useRedirect } from '../../hooks/useRedirect'
import { useApiCall, ApiMethods } from '../../hooks/apiCall'
import { ValidationError } from '../../components/ValidationError'
import { IPhoneNumberRequest, IVerificationCodeResponse } from '../../models/AuthModels'
import { PreLoginContext } from '../../context/PreLoginContext'
import { IValidationErrorResponse } from '../../models/ErrorModels'
import { Button } from 'react-bootstrap'
import { GlobalStrings } from '../../models/Values'
import { useJwtData } from '../../hooks/jwtData'

export function EnterPhone() {
    const [phoneNumberValue, setphoneNumberValue] = useState('')
    const [error, setError] = useState('')
    const sendSmsCall = useApiCall<IPhoneNumberRequest, IVerificationCodeResponse>("auth/sendsmscode", ApiMethods.POST)
    const preLoginContext = useContext(PreLoginContext)
    const redirect = useRedirect()
    const jwtData = useJwtData()

    useEffect(() => {
        if (preLoginContext.phoneNumber.length > 0) {
            setphoneNumberValue(preLoginContext.phoneNumber)
        } else {
            var jwt = jwtData.getData()
            if (jwt && jwt.phoneNumber) {
                setphoneNumberValue(jwt.phoneNumber)

                jwtData.clear()
            }
        }
    }, [])
    

    const submitHandler = async (event: React.FormEvent) => {
        event.preventDefault()
        setError('')

        const phoneRegExp = new RegExp('^\\+\\d{1,3}\\({0,1}\\d{2,3}\\){0,1}\\d{7}$')
        if (phoneNumberValue.trim().length === 0 || !phoneRegExp.test(phoneNumberValue)) {
            setError('Введите номер телефона в формате +375(xx)xxxxxxx.')
            return
        }

        const request: IPhoneNumberRequest = {
            phoneNumber: phoneNumberValue
        }

        const response = (await sendSmsCall.makeRequest(request, false))
        if (response.response !== null) {
            preLoginContext.phoneNumber = phoneNumberValue
            preLoginContext.verificationCode = response.response.code

            redirect.redirectToPage('/auth/code')
        }
        else {
            if (response.apiError.validationErrors) {
                const validatioErrors = response.apiError as IValidationErrorResponse
                setError(validatioErrors.validationErrors[0].message)
            }
            else {
                alert(response.apiError)
            }
        }
    }

    const changeHandler = (event: React.ChangeEvent<HTMLInputElement>) => {
        setphoneNumberValue(event.target.value)
    }

    return (
        <form onSubmit={submitHandler}>
            <label>Номер телефона</label>
            <input
                type="text"
                placeholder="+375(xx)xxxxxxx"
                value={phoneNumberValue}
                onChange={changeHandler}
            />

            {error && <ValidationError error={error} />}

            <Button variant="outline-dark" type="submit">Отправить</Button>
        </form>
    )
}