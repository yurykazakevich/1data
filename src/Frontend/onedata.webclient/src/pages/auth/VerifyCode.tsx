import React, { useState, useContext } from 'react'
import { useApiCall, ApiMethods } from '../../hooks/apiCall'
import { ValidationError } from '../../components/ValidationError'
import { ILoginReuest, ITokenResponse } from '../../models/AuthModels'
import { PreLoginContext } from '../../context/PreLoginContext'

export function VerifyCode() {
    const [value, setValue] = useState('')
    const [error, setError] = useState('')
    const sendSmsCall = useApiCall<ILoginReuest, ITokenResponse>("auth/login", ApiMethods.POST)
    const preLoginContext = useContext(PreLoginContext)

    const submitHandler = async (event: React.FormEvent) => {
        event.preventDefault()
        setError('')

        var codeLength = parseInt(process.env.REACT_APP_VERIFICATION_CODE_LENGTH as string)

        if (value.trim().length !== codeLength) {
            setError('Код должен состоять из ' + codeLength + '-ти цифр')
            return
        }

        const request: ILoginReuest = {
            phoneNumber: preLoginContext.phoneNumber,
            userProvidedCode: value,
            verificationCode: preLoginContext.verificationCode
        }

        const response = (await sendSmsCall.makeRequest(request))

        if (response.response !== null) {

        }
    }

    const changeHandler = (event: React.ChangeEvent<HTMLInputElement>) => {
        setValue(event.target.value)
    }

    return (
        <form onSubmit={submitHandler}>
            <label>Введите проверочный код из СМС</label>
            <input
                type="text"
                className="border py-2 px-4 mb-2 w-full outline-0"
                value={value}
                onChange={changeHandler}
            />

            {error && <ValidationError error={error} />}

            <button type="submit" className="py-2 px-4 border bg-yellow-400 hover:text-white">Отправить</button>
        </form>
    )
}