import qs from 'qs'

// const baseUrl = 'http://localhost:5000'
const baseUrl = 'http://ec2-18-156-78-200.eu-central-1.compute.amazonaws.com'

export class Content {

    public constructor(public type: string, public body: BodyInit | null) {
    }
}

export type ContentType = unknown | Content


export function get<T>(
    path: string,
    data: ContentType,
    onSuccess?: (value: T) => void
) {

    if (data !== undefined) {
        
        const query = qs.stringify(data)

        if (query) {
            path += '?' + qs.stringify(data, { skipNulls: true })
        }
    }

    const url = `${baseUrl}/${path}`

    chrome.runtime.sendMessage(
        {
            method: "GET",
            url
        },
        response => {
            if (response != undefined && response != "") {
                
                onSuccess(JSON.parse(response))
            }
        });
}



export function put<T>(
    path: string,
    data: ContentType,
    onSuccess?: (value: T) => void
) {

    const url = `${baseUrl}/${path}`

    chrome.runtime.sendMessage(
        {
            method: "PUT",
            url,
            data: data ? JSON.stringify(data) : undefined
        },
        response => {
            if (response != undefined && response != "") {
                
                onSuccess(JSON.parse(response))
            }
        });
}