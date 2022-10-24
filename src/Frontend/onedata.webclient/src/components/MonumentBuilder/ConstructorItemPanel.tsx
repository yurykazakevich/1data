import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Button from 'react-bootstrap/Button';
import { CItemTypes } from '../../models/Values';

function ConstructorItemPanel(props: { showItemList: (itemType: CItemTypes) => void }) {
    const { showItemList } = props

  return (
      <Container fluid className='overflow-hidden p-0 h-100 item-panel'>
          <Row className="p-1 item-panel-row">
              <Col>
                  <Button variant="outline-dark" className='btn-block' size="sm"
                      onClick={() => { showItemList(CItemTypes.Pedestal) }}>Тумба</Button>{' '}
              </Col>
          </Row>
          <Row className="p-1 item-panel-row">
              <Col>
                  <Button variant="outline-dark" className='btn-block' size="sm">Стела</Button>{' '}
              </Col>
          </Row>
          <Row className="p-1 item-panel-row">
              <Col>
                  <Button variant="outline-dark" className='btn-block' size="sm">Цветник</Button>{' '}
             </Col>
          </Row>
          <Row className="p-1 item-panel-row">
              <Col>
                  <Button variant="outline-dark" className='btn-block' size="sm">Надгробие</Button>{' '}
              </Col>
          </Row>
          <Row className="p-1 item-panel-row">
              <Col>
                  <Button variant="outline-dark" className='btn-block' size="sm">Ограда</Button>{' '}
              </Col>
          </Row>
          <Row className="p-1 item-panel-row">
              <Col>
                  <Button variant="outline-dark" className='btn-block' size="sm">Наконечники</Button>{' '}
              </Col>
          </Row>
          <Row className='py-1'>
              <Col>
                  <Button variant="outline-dark" className='btn-block' size="sm">Скамья</Button>{' '}
              </Col>
          </Row>
      </Container>
  );
}

export default ConstructorItemPanel;