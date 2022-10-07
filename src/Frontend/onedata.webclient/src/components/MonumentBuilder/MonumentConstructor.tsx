import React, { useState, useEffect } from 'react';
import { BurialTypes, CItemTypes } from '../../models/Values';
import ConstructorItemPanel from './ConstructorItemPanel';
import Col from 'react-bootstrap/Col';
import Image from 'react-bootstrap/Image'
import { useApiCall, ApiMethods } from '../../hooks/apiCall'
import { IGetBgImageRequest } from '../../models/MonumentBuilder';
import { useUrlBuilder } from '../../hooks/urlBuilder';
import { IImageUrlResponse } from '../../models/Global';
import ConstructorItemList from './ConstructorItmeList';

enum ConstructorRightViewTypes {
    ItemPanel,
    ItemList,
    ItemDetails
}

interface IImageUrls {
    background: string | undefined,
    items: { [key: string]: string },
    pedestal: string | undefined,  // Тумба
    garden: string | undefined,    // Цветник
    stele: string | undefined,    // Стелла
    tombston: string | undefined, // Надгробие
    boder: string | undefined,     // Ограда
    tip: string | undefined,       // Наконечник
    bench: string | undefined,     // Скамья
    vase: string | undefined,      // Ваза
    lampada: string | undefined,    // Лампада
}

function MonumentConstructor(props: { centerColumnWidth: number }) {
    const { centerColumnWidth } = props
    const urlBuilder = useUrlBuilder()
    const [rightViewType, setRightViewType] = useState(ConstructorRightViewTypes.ItemPanel)
    const [burialType, setBurialType] = useState(BurialTypes.Single)
    const getBgImage = useApiCall<IGetBgImageRequest, IImageUrlResponse>("image/background", ApiMethods.GET)
    const [imageUrls, setImageUrls] = useState({
        background: undefined,
        pedestal: undefined,  // Тумба
        garden: undefined,    // Цветник
        stele: undefined,    // Стелла
        tombston: undefined, // Надгробие
        boder: undefined,     // Ограда
        tip: undefined,       // Наконечник
        bench: undefined,     // Скамья
        vase: undefined,      // Ваза
        lampada: undefined
    } as IImageUrls)

    function isItemPanel(): boolean {
        return rightViewType === ConstructorRightViewTypes.ItemPanel
    }

    function isItemList(): boolean {
        return rightViewType === ConstructorRightViewTypes.ItemList
    }

    function isItemDetails(): boolean {
        return rightViewType === ConstructorRightViewTypes.ItemDetails
    }

    function showItemPanel() {
        setRightViewType(ConstructorRightViewTypes.ItemPanel)
    }

    function showItemList(itemType: string) {
        setRightViewType(ConstructorRightViewTypes.ItemList)
    }

    function showItemDetails(itemId: number) {
        setRightViewType(ConstructorRightViewTypes.ItemDetails)
    }

    useEffect(() => {
        var request: IGetBgImageRequest = {
            burialType: burialType
        }

        var bgImageUrl = async () => {
            const apiResponse = await getBgImage.makeRequest(request)
            //TODO: apiResponse.apiError

            if (apiResponse.response !== null && apiResponse.response.imageUrl) {
                const imagePath: string = apiResponse.response.imageUrl
                const imageUrl = urlBuilder.buildApiUrl(imagePath)

               setImageUrls({ ...imageUrls, background: imageUrl })
            }
        }

        bgImageUrl()
    },[])

    return (
    <>
        <Col lg={centerColumnWidth} className="constructor-image-container">
            {imageUrls.background && <Image src={imageUrls.background} className="position-relative top-0 start-0" />}
            {imageUrls.pedestal && <Image src={imageUrls.pedestal} className="position-relative top-0 start-0" />}
        </Col>
        <Col>
            {isItemPanel() && <ConstructorItemPanel showItemList={ showItemList } />}
                {isItemList() && <ConstructorItemList showItemDetails={showItemDetails} backToMenu={showItemPanel} />}
            {isItemDetails() && <p>Item Details</p>}
        </Col>
    </>
  );
}

export default MonumentConstructor;