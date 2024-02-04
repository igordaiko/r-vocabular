import React from 'react'
import { Root, createRoot } from "react-dom/client";
import { Popover } from './popover';

let root: Root;

export function initialize(toTranlate: string) {
    
    const container = document.getElementById('rvocabular-root') as HTMLElement
    root = root ?? createRoot(container)

    root.render(<Popover value={toTranlate} />)
};


export function dispose() {
    root?.unmount()

    root = undefined
}