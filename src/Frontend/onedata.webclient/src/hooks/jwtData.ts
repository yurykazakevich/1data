import { ITokenResponse } from "../models/AuthModels";
import { GlobalStrings } from "../models/Values";

export function useJwtData() {

    function getData(): ITokenResponse | null {
        var data: ITokenResponse | null = null
        const jwtDataString = sessionStorage.getItem(GlobalStrings.jwtDataKey)
        if (jwtDataString !== null) {
            data = JSON.parse(jwtDataString)
        }

        return data
    }

    function setData(data: ITokenResponse) {
        sessionStorage.setItem(GlobalStrings.jwtDataKey, JSON.stringify(data))
    }

    function clear() {
        sessionStorage.removeItem(GlobalStrings.jwtDataKey)
    }

    return { getData, setData, clear };
}