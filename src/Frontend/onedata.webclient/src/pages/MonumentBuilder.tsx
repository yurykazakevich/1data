import React from 'react';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import { useState } from 'react'
import Col from 'react-bootstrap/Col';
import Image from 'react-bootstrap/Image'
import MonumentCatalog from '../components/MonumentBuilder/MonumentCatalog';
import MonumentStela from '../components/MonumentBuilder/MonumentStela';
import MonumentConstructor from '../components/MonumentBuilder/MonumentConstructor';
import MonumentBase from '../components/MonumentBuilder/MonumentBase';
import MonumentResult from '../components/MonumentBuilder/MonumentResult';
import { Button } from 'react-bootstrap';

function MonumentBuilder() {
    const [ stepNumber, setStepNumber ] = useState(2)

    function nextStep() {
        if (stepNumber < 5) {
            setStepNumber(prevStepNumber => prevStepNumber + 1)
        }
    }

    function previousStep() {
        if (stepNumber > 1) {
            setStepNumber(prevStepNumber => prevStepNumber - 1)
        }
    }

    function getStepNumberClass(dispaliedNumber: number): string {
        return dispaliedNumber <= stepNumber
            ? 'prev-step-number'
            : 'next-step-number'
    }

    return (
      <Container fluid="xl">
          <Row>
              <Col sm={'auto'} className={ 'p-1' }>
                  <div className={'step-number-container text-center'}>
                        <Button variant="link" onClick={previousStep} disabled={ stepNumber === 1 }>
                            <Image src="images/return.svg" />
                        </Button>   
                  </div>
              </Col>
                <Col lg={9}>
                    <h5>
                        {stepNumber === 1 && <>Каталог</>}
                        {stepNumber === 2 && <>Конструктор памника</>}
                        {stepNumber === 3 && <>Оформление стелы</>}
                        {stepNumber === 4 && <>Установка памятника</>}
                        {stepNumber === 5 && <>Ваш заказ</>}
                    </h5>
                </Col>
              <Col className={'text-right'}>
                  <Image src="images/headset.svg" />
              </Col>
          </Row>
          <Row>
              <Col sm={'auto'} className={'p-1'}>
                  <div className={'step-number-container text-center'}>
                      <Container>
                          <Row>
                                <Col className={getStepNumberClass(1) + ' py-1'}>1</Col>
                          </Row>
                          <Row>
                                <Col className={getStepNumberClass(2) + ' py-1'}>2</Col>
                          </Row>
                          <Row>
                                <Col className={getStepNumberClass(3) + ' py-1'}>3</Col>
                          </Row>
                          <Row>
                                <Col className={getStepNumberClass(4) + ' py-1'}>4</Col>
                          </Row>
                          <Row>
                                <Col className={getStepNumberClass(5) + ' py-1'}>5</Col>
                          </Row>
                      </Container>
                  </div>
              </Col>
                {stepNumber === 1 && <MonumentCatalog />}
                {stepNumber === 2 && <MonumentConstructor centerColumnWidth={9} />}
                {stepNumber === 3 && <MonumentStela />}
                {stepNumber === 4 && <MonumentBase />}
                {stepNumber === 5 && <MonumentResult />}
          </Row>
          <Row>
                <Col sm={'auto'} className={'p-1'}>
                  <div className={'step-number-container'}>
                  </div>
                </Col>
                <Col lg={9}>
                    <Button variant="dark" size="sm"
                        disabled={stepNumber === 5} onClick={nextStep}>
                        Продолжить
                    </Button>
                </Col>
                <Col>ИТОГО</Col>
          </Row>
      </Container>
  );
}

export default MonumentBuilder;