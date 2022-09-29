import React, { useState } from 'react';
import { BurialTypes } from '../../models/Values';
import ConstructorItemPanel from './ConstructorItemPanel';
import Col from 'react-bootstrap/Col';

enum ConstructorRightViewTypes {
    ItemPanel,
    ItemList,
    ItemDetails
}

function MonumentConstructor(props: { centerColumnWidth: number }) {
    const { centerColumnWidth } = props

    const [rightViewType, setRightViewType] = useState(ConstructorRightViewTypes.ItemPanel)
    const [burialType, setBurialType] = useState(BurialTypes.Single)

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

    return (
    <>
        <Col lg={centerColumnWidth}>Center</Col>
        <Col>
            {isItemPanel() && <ConstructorItemPanel showItemList={ showItemList } />}
            {isItemList() && <p>Item List</p>}
            {isItemDetails() && <p>Item Details</p>}
        </Col>
    </>
  );
}

export default MonumentConstructor;