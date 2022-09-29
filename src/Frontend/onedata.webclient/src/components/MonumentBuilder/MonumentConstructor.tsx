import React, { useState } from 'react';
import ConstructorItemPanel from './ConstructorItemPanel';
import Col from 'react-bootstrap/Col';

export enum ConstructorRightViewTypes {
    ItemPanel,
    ItemList,
    ItemDetails
}

function MonumentConstructor(props: { centerColumnWidth: number }) {
    const { centerColumnWidth } = props

    const [rightViewType, setRightViewType] = useState(ConstructorRightViewTypes.ItemPanel)

    function isItemPanel(): boolean {
        return rightViewType === ConstructorRightViewTypes.ItemPanel
    }

    function isItemList(): boolean {
        return rightViewType === ConstructorRightViewTypes.ItemList
    }

    function isItemDetails(): boolean {
        return rightViewType === ConstructorRightViewTypes.ItemDetails
    }

    function showItemList(itemType: string) {
        setRightViewType(ConstructorRightViewTypes.ItemList)
    }

    return (
    <>
            <Col lg={centerColumnWidth}>Center</Col>
        <Col>
            {isItemPanel() && <ConstructorItemPanel showItemList={ showItemList } />}
            {isItemList() && <p>Item List</p>}
        </Col>
    </>
  );
}

export default MonumentConstructor;