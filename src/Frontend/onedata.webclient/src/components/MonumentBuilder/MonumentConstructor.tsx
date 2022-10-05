import React, { useState, useRef, useEffect } from 'react';
import { BurialTypes } from '../../models/Values';
import ConstructorItemPanel from './ConstructorItemPanel';
import Col from 'react-bootstrap/Col';
import Image from 'react-bootstrap/Image'
import { useApiCall, ApiMethods } from '../../hooks/apiCall'
import { IGenBgImageRequest } from '../../models/MonumentBuilder';

enum ConstructorRightViewTypes {
    ItemPanel,
    ItemList,
    ItemDetails
}

function MonumentConstructor(props: { centerColumnWidth: number }) {
    const { centerColumnWidth } = props

    const [rightViewType, setRightViewType] = useState(ConstructorRightViewTypes.ItemPanel)
    const [burialType, setBurialType] = useState(BurialTypes.Single)
    const bgImage = useRef<HTMLImageElement>(null)
    const getBgImage = useApiCall<IGenBgImageRequest, Blob>("image/background", ApiMethods.GET)

    function isItemPanel(): boolean {
        return rightViewType === ConstructorRightViewTypes.ItemPanel
    }

    function isItemList(): boolean {
        return rightViewType === ConstructorRightViewTypes.ItemList
    }

    function isItemDetails(): boolean {
        return rightViewType === ConstructorRightViewTypes.ItemDetails
    }

    function showItemPanel(itemType: string) {
        setRightViewType(ConstructorRightViewTypes.ItemPanel)
    }

    function showItemList(itemType: string) {
        setRightViewType(ConstructorRightViewTypes.ItemList)
    }

    function showItemDetails(itemType: string) {
        setRightViewType(ConstructorRightViewTypes.ItemDetails)
    }

    useEffect(() => {
        var request: IGenBgImageRequest = {
            burialType: burialType
        }

        const fetchData = async () => {
            const apiResponse = await getBgImage.makeRequest(request)

            //TODO: apiResponse.apiError

            if (apiResponse.response !== null) {
                const image: Blob = apiResponse.response
                const imageUrl = URL.createObjectURL(image)

                const bgImageElement = bgImage.current
                if (bgImageElement !== null) {
                    bgImageElement.onload = () => URL.revokeObjectURL(imageUrl)
                    bgImageElement.src = imageUrl
                }

            }
        }

        fetchData()

        


    },[])

    return (
    <>
            <Col lg={centerColumnWidth}>
                <Image ref={bgImage} />
            </Col>
        <Col>
            {isItemPanel() && <ConstructorItemPanel showItemList={ showItemList } />}
            {isItemList() && <p>Item List</p>}
            {isItemDetails() && <p>Item Details</p>}
        </Col>
    </>
  );
}

export default MonumentConstructor;