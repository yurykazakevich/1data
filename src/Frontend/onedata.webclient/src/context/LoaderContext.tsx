import React, { createContext, useState } from 'react'


interface ILoaderContext {
    showLoader: () => void
    hideLoader: () => void
}

export const LoaderContext = createContext<ILoaderContext>({
    showLoader: () => { },
    hideLoader: () => { }
})

export const LoaderState = () => {
    const [visible, setVisible] = useState(false)

    const showLoader = () => setVisible(true)

    const hideLoader = () => setVisible(false)

    return (
        <LoaderContext.Provider value={{ showLoader, hideLoader }}>
            {visible && <>
                <div className="fixed bg-black/50 top-0 right-0 left-0 bottom-0" />
                <div className="w-[500px] p-5 rounded bg-white absolute top-10 left-1/2 -translate-x-1/2">
                    <p className="text-center">Loading...</p>
                </div>
            </>}
        </LoaderContext.Provider>
    )
}