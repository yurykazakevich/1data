import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import { LoaderState } from './context/LoaderContext';
import { BrowserRouter } from 'react-router-dom'
import { ModalState } from './context/ModalContext';

const root = ReactDOM.createRoot(
    document.getElementById('root') as HTMLElement


);
root.render(
    <React.StrictMode>
        <LoaderState>
            <BrowserRouter>
                <App />
            </BrowserRouter>
        </LoaderState>
    </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
