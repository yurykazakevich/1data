import { useEffect, useState } from "react";
import { Button, Col, Container, Row } from "react-bootstrap";
import { ApiMethods, useApiCall } from "../../hooks/apiCall";
import { IItemListRequest, IListItemResponse, ISelectedCItem } from "../../models/MonumentBuilder";
import { BurialPositions, BurialTypes, CItemTypes } from "../../models/Values";
import ConstructorItemCard from "./ConstructorItemCard";

export interface IConstructorItemListProps {
    itemType: CItemTypes,
    burialType: BurialTypes,
    burialPosition: BurialPositions
    showItemDetails: (itemId: number) => void,
    handleItemSelected: (selectedItem: ISelectedCItem) => void,
    backToMenu: () => void
}

function ConstructorItemList(props: IConstructorItemListProps) {
    const showItemDetails = props.showItemDetails
    const backToMenu = props.backToMenu
    const getItemList = useApiCall<IItemListRequest, IListItemResponse[]>("constructoritem", ApiMethods.GET)
    const [itemListData, setItemListData] = useState(new Array<IListItemResponse[]>())
    const [selectedItemId, setSelectedItemId] = useState(0)

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

        callGetItemList()

    }, [props.itemType, props.burialPosition, props.burialType])

    function handleItemSelected(selectedItem: ISelectedCItem) {
        if (selectedItem.id === selectedItemId) {
            setSelectedItemId(0)
        } else {
            setSelectedItemId(selectedItem.id)
        }

        props.handleItemSelected(selectedItem)
    }

    return (
        <Container fluid className="p-0">
            <Row>
                <Col className="p-1">
                    <Button variant="outline-primary" className='btn-block' size="sm" onClick={backToMenu}>Назад в меню</Button>{' '}
                </Col>
            </Row>
            {itemListData.map(itemRow =>
                <Row>
                    {itemRow.map(item => 
                        <Col xs="6" className="p-1">
                            <ConstructorItemCard
                                item={item}
                                selectedItemId={selectedItemId}
                                showItemDetails={showItemDetails}
                                handleItemSelected={handleItemSelected} />
                        </Col>)}
                </Row>
            )}
        </Container>
    );
}

export default ConstructorItemList;