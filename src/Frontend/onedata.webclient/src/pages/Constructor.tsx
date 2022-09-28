import React from 'react';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import { useState } from 'react'
import Col from 'react-bootstrap/Col';
import Image from 'react-bootstrap/Image'
import ConstructorItemPanel from '../components/ConstructorItemPanel';

export enum ConstructorRightViewTypes {
    ItemPanel,
    ItemList,
    ItemDetails
}

function Constructor() {
    const [ rightViewType, setRightViewType ] = useState(ConstructorRightViewTypes.ItemPanel)

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
      <Container fluid="xl">
          <Row>
              <Col sm={'auto'} className={ 'p-1' }>
                  <div className={'step-number-container text-center'}>
                      <Image src="images/return.svg" />   
                  </div>
              </Col>
              <Col lg={9}>Конструктор памятника</Col>
              <Col className={'text-right'}>
                  <Image src="images/headset.svg" />
              </Col>
          </Row>
          <Row>
              <Col sm={'auto'} className={'p-1'}>
                  <div className={'step-number-container text-center'}>
                      <Container>
                          <Row>
                              <Col>1</Col>
                          </Row>
                          <Row>
                              <Col>2</Col>
                          </Row>
                          <Row>
                              <Col>3</Col>
                          </Row>
                          <Row>
                              <Col>4</Col>
                          </Row>
                          <Row>
                              <Col>5</Col>
                          </Row>
                      </Container>
                  </div>
              </Col>
              <Col lg={9}>Center</Col>
              <Col>
                    {isItemPanel() && <ConstructorItemPanel showItemList={ showItemList } />}
                    {isItemList() && <p>Item List</p>}
              </Col>
          </Row>
          <Row>
              <Col sm={'auto'} className={'p-1'}>
                  <div className={'step-number-container'}>
                  </div>
              </Col>
              <Col lg={9}>Center</Col>
              <Col>ИТОГО</Col>
          </Row>
      </Container>
  );
}

export default Constructor;