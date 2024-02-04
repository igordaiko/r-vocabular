export interface TranslationResponse {
    mainResult: TranslationItem,
    synonyms: TranslationItem[]

}

export interface TranslationItem {
    value: string,
    translatedText: string,
    partsOfSpeech: PartOfSpeech[],
    definitions: Definition[]
}

export enum PartOfSpeech {
    Noun,
    Verb,
    Adjective,
    Adverb,
    None
}

export interface Definition {
    partOfSpeech: PartOfSpeech,
    value: string,
    tags: string[]
}

export interface AddWordRequest {
    word: string,
    translation?: string,
    definition?: string
}