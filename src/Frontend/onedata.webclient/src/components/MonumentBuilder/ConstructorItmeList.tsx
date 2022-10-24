import { useEffect, useState } from "react";
import { ApiMethods, useApiCall } from "../../hooks/apiCall";
import { IItemListRequest, IListItemResponse } from "../../models/MonumentBuilder";
import { BurialPositions, BurialTypes, CItemTypes } from "../../models/Values";

export interface IConstructorItemListProps {
    itemType: CItemTypes,
    burialType: BurialTypes,
    burialPosition: BurialPositions
    showItemDetails: (itemId: number) => void,
    backToMenu: () => void
}

function ConstructorItemList(props: IConstructorItemListProps) {
    const showItemDetails = props.showItemDetails
    const backToMenu = props.backToMenu
    const getItemList = useApiCall<IItemListRequest, IListItemResponse[]>("constructoritem", ApiMethods.GET)
    const [itemListData, setItemListData] = useState(new Array<IListItemResponse[]>())

    useEffect(() => {
        var request: IItemListRequest = {
            burialPosition: props.burialPosition,
            burialType: props.burialType,
            itemType: props.itemType
        }

        var callGetItemList = async () => {
            const apiResponse = await getItemList.makeRequest(request)
            //TODO: apiResponse.apiError

            if (apiResponse.response !== null) {
                var itemList = apiResponse.response as IListItemResponse[]
                var listData: IListItemResponse[][] = new Array(Math.ceil(itemList.length/2))

                for (var i = 0; i < itemList.length; i++) {
                    var listRow: IListItemResponse[] = new Array(2)
                    listRow.push(itemList[i])
                    i++
                    if (i < itemList.length) {
                        listRow.push(itemList[i])
                    }

                    listData.push(listRow)
                }

                setItemListData(listData)
            }
        }
    }, [props.itemType, props.burialPosition, props.burialType])

    return (
        <>
            {itemListData.map(itemRow => itemRow[0].id)}
        </>
    );
}

export default ConstructorItemList;