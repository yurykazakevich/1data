import React, { useState, useContext, useEffect } from 'react'
import { useRedirect } from '../../hooks/useRedirect'
import { useApiCall, ApiMethods } from '../../hooks/apiCall'
import { ValidationError } from '../../components/ValidationError'
import { IPhoneNumberRequest, IVerificationCodeResponse } from '../../models/AuthModels'
import { PreLoginContext } from '../../context/PreLoginContext'
import { IJwtContext, JwtContext } from '../../context/JwtContext'
import { IValidationErrorResponse } from '../../models/ErrorModels'

export function EnterPhone() {
    const [phoneNumberValue, setphoneNumberValue] = useState('')
    const [error, setError] = useState('')
    const sendSmsCall = useApiCall<IPhoneNumberRequest, IVerificationCodeResponse>("auth/sendsmscode", ApiMethods.POST)
    const preLoginContext = useContext(PreLoginContext)
    const redirect = useRedirect()
    const jwtContext: IJwtContext = useContext(JwtContext)

    useEffect(() => {
        if (preLoginContext.phoneNumber.length > 0) {
            setphoneNumberValue(preLoginContext.phoneNumber)
        } else if (jwtContext.data?.phoneNumber.length > 0) {
            setphoneNumberValue(preLoginContext.phoneNumber)
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

        const response = (await sendSmsCall.makeRequest(request))
        if (response.response !== null) {
            preLoginContext.phoneNumber = phoneNumberValue
            preLoginContext.verificationCode = response.response.code

            redirect.redirectToPage('/auth/code')
        }
        else {
            const validatioErrors = response.apiError as IValidationErrorResponse
            if (validatioErrors !== null) {
                setError(validatioErrors.errors[0].message)
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
                className="border py-2 px-4 mb-2 w-full outline-0"
                placeholder="+375(xx)xxxxxxx"
                value={phoneNumberValue}
                onChange={changeHandler}
            />

            {error && <ValidationError error={error} />}

            <button type="submit" className="py-2 px-4 border bg-yellow-400 hover:text-white">Send</button>
        </form>
    )
}