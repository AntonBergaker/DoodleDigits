import React from 'react';
import { createRoot } from 'react-dom/client';
import { CalculatorPage } from '../pages/calculator/calculator-page';


function render() {
    const root = createRoot(document.getElementById("document-root"));

    root.render(<CalculatorPage/>);
}

render();
