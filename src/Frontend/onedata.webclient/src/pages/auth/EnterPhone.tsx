import React, { useState, useContext, useEffect, useRef } from 'react'
import { useRedirect } from '../../hooks/redirect'
import { useApiCall, ApiMethods } from '../../hooks/apiCall'
import { IPhoneNumberRequest, IVerificationCodeResponse } from '../../models/AuthModels'
import { PreLoginContext } from '../../context/PreLoginContext'
import { IValidationErrorResponse } from '../../models/ErrorModels'
import { Button, Form, InputGroup } from 'react-bootstrap'
import { useJwtData } from '../../hooks/jwtData'

type Values = {
    userType: number,
    phoneNumber: string,
    personalData: boolean,
}

export function EnterPhone() {
    const defaultPhoneNumberErrorMssage = 'Введите номер телефона в формате +375(xx)xxxxxxx.'

    const [phoneNumberErrorMessage, setPhoneNumberErrorMessage] = useState(defaultPhoneNumberErrorMssage)
    const [isPhoneNumberValid, setIsPhoneNumberValid] = useState(true)
    const [isPersonalDataValid, setIsPersonalDataValid] = useState(true)
    const sendSmsCall = useApiCall<IPhoneNumberRequest, IVerificationCodeResponse>("auth/sendsmscode", ApiMethods.POST)
    const preLoginContext = useContext(PreLoginContext)
    const redirect = useRedirect()
    const jwtData = useJwtData()

    const [values, setValues] = useState<Values>({
        userType: 1,
        phoneNumber: "",
        personalData: false,
    });

    useEffect(() => {
        var initialValues: Values = {
            userType: 1,
            phoneNumber: "",
            personalData: false,
        }

        if (preLoginContext.phoneNumber.length > 0) {
            initialValues.phoneNumber = preLoginContext.phoneNumber
            initialValues.userType = preLoginContext.isOrg ? 0 : 1
        } else {
            var jwt = jwtData.getData()
            if (jwt && jwt.phoneNumber) {
                initialValues.phoneNumber = jwt.phoneNumber
                initialValues.userType = jwt.isPhisical ? 1 : 0
                jwtData.clear()
            }
        }

        setValues(initialValues)
    }, [])
    

    const submitHandler = async () => {
        var isValid = true
        setIsPhoneNumberValid(true)
        setPhoneNumberErrorMessage(defaultPhoneNumberErrorMssage)
        setIsPersonalDataValid(true)

        if (!values.personalData) {
            isValid = false
            setIsPersonalDataValid(false)
        }
        const phoneRegExp = new RegExp('^\\+\\d{1,3}\\({0,1}\\d{2,3}\\){0,1}\\d{7}$')
        if (values.phoneNumber.trim().length == 0 || !phoneRegExp.test(values.phoneNumber)) {
            setIsPhoneNumberValid(false)
            isValid = false
        }

        if (isValid) {
            const request: IPhoneNumberRequest = {
                phoneNumber: values.phoneNumber,
                isPhisical: values.userType == 1
            }

            const response = (await sendSmsCall.makeRequest(request, false))
            if (response.response !== null) {
                preLoginContext.phoneNumber = values.phoneNumber
                preLoginContext.verificationCode = response.response.code
                preLoginContext.isOrg = values.userType == 0

                redirect.redirectToPage('/auth/code')
            }
            else {
                if (response.apiError.validationErrors) {
                    const validatioErrors = response.apiError as IValidationErrorResponse
                    setPhoneNumberErrorMessage(validatioErrors.validationErrors[0].message)
                    setIsPhoneNumberValid(false)
                }
                else {
                    alert(response.apiError)
                }
            }
        }
    }
    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        if (event.target.type === "checkbox") {
            setValues({ ...values, [event.target.name]: event.target.checked });
        } else {
            setValues({ ...values, [event.target.name]: event.target.value });
        }
    }
    const handleSelectChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
        setValues({ ...values, [event.target.name]: event.target.value });
    }

    return (
        <>
            <div className="text-center">
                <h2>Введите ваш номер</h2>
                <p>На Ваш номер телефона Вы получите код</p>
            </div>
            <InputGroup className="mb-3">
                <InputGroup.Text className="custom-item-group">Вы:</InputGroup.Text>
                <Form.Select name="userType" onChange={handleSelectChange} value={values.userType} >
                    <option value="1">Физ.лицо</option>
                    <option value="0">Юр. лицо</option>
                </Form.Select>
            </InputGroup>
            <InputGroup className="mb-3" hasValidation>
                <InputGroup.Text className="custom-item-group">Ваш телефон:</InputGroup.Text>
                <Form.Control
                    name="phoneNumber"
                    placeholder="+375(xx)xxxxxxx"
                    isInvalid={!isPhoneNumberValid}
                    onChange={handleChange}
                    value={values.phoneNumber} />
                <Form.Control.Feedback type="invalid">
                    { phoneNumberErrorMessage }
                </Form.Control.Feedback>
            </InputGroup>
            <InputGroup className="mb-3">
                <Form.Check type="checkbox"
                    name="personalData"
                    label="Соглашаюсь на обработку персональных данных"
                    onChange={handleChange}
                    isInvalid={!isPersonalDataValid} />
            </InputGroup>
            <div className="d-grid gap-2">
                <Button variant="dark" type="button" size="lg" onClick={submitHandler}>
                    Отправить
                </Button>
            </div>
        </>
    )
}