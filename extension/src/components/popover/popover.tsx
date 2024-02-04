import React, { useState } from "react"

import { Button, Drawer, Input, Tag } from 'antd'
import { PlusOutlined, CheckOutlined } from '@ant-design/icons';

import { get, put } from '../../utils/http-requests'

import { dispose } from "."
import { AddWordRequest, Definition, TranslationResponse } from "../../api/contracts";



export function Popover({ value: wordToTranslate }: { value: string }) {

    const [open, setOpen] = useState(false)
    const [showButton, setShowButton] = useState(true)
    const [translation, setTranslation] = useState({} as TranslationResponse)
    const [customTranslation, setCustomTranslation] = useState(undefined as string)
    const [translatinAdded, setTranslationAdded] = useState(false)

    const showDrawer = async () => {

        get('translate', { word: wordToTranslate }, (value: TranslationResponse) => {
            setTranslation(value)

            setOpen(true);
        })

        setShowButton(false)
    };

    const onClose = () => {
        setOpen(false)
        dispose()
    };

    const getDefinitionTags = (definition: Definition) => {
        const tags = [...definition.tags] ?? []

        if (definition.partOfSpeech) {
            tags.unshift(definition.partOfSpeech.toString())
        }

        return tags
    }

    const saveWord = (translation: string) => {
        
        const request: AddWordRequest = {
            word: wordToTranslate,
            translation: translation
        }

        put('add', request, () => setTranslationAdded(true))
    }

    const capitalize = (string: string) => string?.charAt(0).toUpperCase() + string?.slice(1)

    return <>
        {showButton && <div 
            id="translate-btn"
            className="rvocabular-translate-button" 
            onClick={showDrawer}
        >
            T
        </div>}

        {<Drawer id="rvocabular-drawer" title="Translation" placement="right" onClose={onClose} open={open} className="rvocabular-drawer">
            <div className="rvocabular-word">{capitalize(translation?.mainResult?.value)}</div>
            
            <div className="rvocabular-block">
                <div>{capitalize(translation?.mainResult?.translatedText)}</div>

                {!translatinAdded && <Button
                    className="rvocabular-add-button"
                    icon={<PlusOutlined onClick={() => saveWord(translation?.mainResult?.translatedText)}/>}
                />}
                
                {translatinAdded && <CheckOutlined />}
            </div>

            <div className="rvocabular-custom-translation-block">
                <Input placeholder="Enter your translation" value={customTranslation} onChange={e => setCustomTranslation(e.target.value)} />
                
                {!translatinAdded && <Button
                    className="rvocabular-add-button"
                    icon={<PlusOutlined onClick={() => saveWord(customTranslation)}/>}
                />}
                {translatinAdded && <CheckOutlined />}
            </div>

            {translation?.mainResult?.definitions?.length && <div className="rvocabular-definitions">
                <div className="rvocabular-subtitle">Definitions: </div>

                {translation.mainResult.definitions.map((x, i) => <div key={i} className="rvocabular-block rvocabular-definition">
                
                    <div>{x.value}</div>
                    
                    <div className="rvocabular-tags">
                        {getDefinitionTags(x).map((tag, i) => <Tag key={`tag-${i}`}>{tag}</Tag>)}
                    </div>
                </div>)}
            </div>}


            {translation?.synonyms && <div className="rvocabular-synonyms">
                <div className="rvocabular-subtitle">Related words:</div>

                {translation.synonyms.map((x, i) => <div key={i} className="rvocabular-block">
                    <span className="rvocabular-synonym-value">{capitalize(x.value)}: </span>
                    <span className="rvocabular-synonym-translation">{capitalize(x.translatedText)}</span>
                </div>
                )}
            </div>}
        </Drawer>}
    </>
}