import React, { createContext, useState } from 'react'
import { Modal, Image } from 'react-bootstrap'

interface ILoaderContext {
    showLoader: () => void
    hideLoader: () => void
}

export const LoaderContext = createContext<ILoaderContext>({
    showLoader: () => { },
    hideLoader: () => { }
})

export const LoaderState = ({ children }: { children: React.ReactNode }) => {
    const [visible, setVisible] = useState(false)

    const showLoader = () => setVisible(true)

    const hideLoader = () => setVisible(false)

    return (
        <LoaderContext.Provider value={{ showLoader, hideLoader }}>
            <Modal show={visible} backdrop="static" keyboard={false} animation={false}
                aria-labelledby="contained-modal-title-vcenter"
                centered size="sm">
                <Modal.Body className="text-center loading" style={{border:"none"}}>
                    <Image src="/images/loading.gif" />
                    <div className="py-2 loading-statement">Загрузка...</div>
                </Modal.Body>
            </Modal>
            { children }
        </LoaderContext.Provider>
    )
}