import { Container, Row, Col } from 'react-bootstrap';
import { Outlet } from 'react-router-dom'
import { PreLoginContext } from '../../context/PreLoginContext';

export function Auth() {
    const phoneNumber: string = ''
    const verificationCode: string = ''
    const isOrg = false

    return (
        <>
            <PreLoginContext.Provider value={{ phoneNumber, verificationCode, isOrg }}>
                <Container fluid="xl">
                    <Row>
                        <Col>
                            <h1>Авторизация</h1>
                        </Col>
                    </Row>
                    <Row>
                        <Col></Col>
                        <Col lg={4}>
                            <Outlet />
                        </Col>
                        <Col></Col>
                    </Row>
                </Container>
             </PreLoginContext.Provider>
        </>
    );
}
