﻿import React, { useState, useContext, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { useApiCall, ApiMethods } from '../../hooks/apiCall'
import { ValidationError } from '../../components/ValidationError'
import { IPhoneNumberRequest, IVerificationCodeResponse } from '../../models/AuthModels'
import { PreLoginContext } from '../../context/PreLoginContext'

export function EnterPhone() {
    const [value, setValue] = useState('')
    const [error, setError] = useState('')
    const sendSmsCall = useApiCall<IPhoneNumberRequest, IVerificationCodeResponse>("auth/sendsmscode", ApiMethods.POST)
    const preLoginContext = useContext(PreLoginContext)
    const navigate = useNavigate()

    useEffect(() => {
        if (preLoginContext.phoneNumber.length > 0) {
            setValue(preLoginContext.phoneNumber)
        }
    }, [])
    

    const submitHandler = async (event: React.FormEvent) => {
        event.preventDefault()
        setError('')

        if (value.trim().length === 0) {
            setError('Введите номер телефона в формате +375(xx)xxxxxxx.')
            return
        }

        const request: IPhoneNumberRequest = {
            phoneNumber: value
        }

        const response = (await sendSmsCall.makeRequest(request))
        if (response.response !== null) {
            preLoginContext.phoneNumber = value
            preLoginContext.verificationCode= response.response.code

            navigate('/auth/code')
        }
    }

    const changeHandler = (event: React.ChangeEvent<HTMLInputElement>) => {
        setValue(event.target.value)
    }

    return (
        <form onSubmit={submitHandler}>
            <label>Номер телефона</label>
            <input
                type="text"
                className="border py-2 px-4 mb-2 w-full outline-0"
                placeholder="+375(xx)xxxxxxx"
                value={value}
                onChange={changeHandler}
            />

            {error && <ValidationError error={error} />}

            <button type="submit" className="py-2 px-4 border bg-yellow-400 hover:text-white">Send</button>
        </form>
    )
}