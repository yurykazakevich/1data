import React, { useState } from 'react'
import { useApiCall, ApiMethods } from '../../hooks/apiCall'
import { ValidationError } from '../../components/ValidationError'
import { IPhoneNumberRequest, IVerificationCodeResponse } from '../../models/AuthModels'

export function ProductsPage() {
    const [value, setValue] = useState('')
    const [error, setError] = useState('')
    const sendSmsCall = useApiCall<IPhoneNumberRequest, IVerificationCodeResponse>("auth/sendsms", ApiMethods.POST)

    const submitHandler = async (event: React.FormEvent) => {
        event.preventDefault()
        setError('')

        if (value.trim().length === 0) {
            setError('Please enter valid phone number.')
            return
        }

        const request: IPhoneNumberRequest = {
            phoneNumber: value
        }

        const response = (await sendSmsCall.makeRequest(request))
    }

    const changeHandler = (event: React.ChangeEvent<HTMLInputElement>) => {
        setValue(event.target.value)
    }

    return (
        <form onSubmit={submitHandler}>
            <input
                type="text"
                className="border py-2 px-4 mb-2 w-full outline-0"
                placeholder="Enter product title..."
                value={value}
                onChange={changeHandler}
            />

            {error && <ValidationError error={error} />}

            <button type="submit" className="py-2 px-4 border bg-yellow-400 hover:text-white">Отправить</button>
        </form>
    )
}