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
          <Row className="flex-nowrap">
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
          <Row className="flex-nowrap">
              <Col sm={'auto'} className={'p-1'}>
                        <Container className="step-number-container text-center h-100">
                        <Row className='py-1 step-number-row'>
                            <Col className="p-0">
                                <div className={getStepNumberClass(1) + " p-1 text-center"}>1</div>
                            </Col>
                          </Row>
                        <Row className='py-1 step-number-row'>
                            <Col className="p-0">
                                <div className={getStepNumberClass(2) + " p-1 text-center"}>2</div>
                            </Col>
                          </Row>
                        <Row className='py-1 step-number-row'>
                            <Col className="p-0">
                                <div className={getStepNumberClass(3) + " p-1 text-center"}>3</div>
                            </Col>
                          </Row>
                        <Row className='py-1 step-number-row'>
                            <Col className="p-0">
                                <div className={getStepNumberClass(4) + " p-1 text-center"}>4</div>
                            </Col>
                          </Row>
                        <Row className='py-1'>
                            <Col className="p-0">
                                <div className={getStepNumberClass(5) + " p-1 text-center"}>5</div>
                            </Col>
                          </Row>
                      </Container>
              </Col>
                {stepNumber == 1 && <MonumentCatalog />}
                {stepNumber == 2 && <MonumentConstructor centerColumnWidth={9} />}
                {stepNumber == 3 && <MonumentStela />}
                {stepNumber == 4 && <MonumentBase />}
                {stepNumber == 5 && <MonumentResult />}
          </Row>
          <Row className="flex-nowrap my-2">
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