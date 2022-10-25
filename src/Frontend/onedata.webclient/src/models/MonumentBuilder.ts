import { BurialTypes, BurialPositions, CItemTypes, ItemPositions } from "./Values";

export interface IGetBgImageRequest {
    burialType: BurialTypes
}

export interface IItemListRequest {
    burialType: BurialTypes,
    burialPosition: BurialPositions,
    itemType: CItemTypes
}

export interface IListItemResponse
{
        id: number,
        name: string,
        size: string,
        price: number,
        currency: string,
        position: ItemPositions,
        image: string,
        categories: string[]
}

export interface ISelectedCItem {
    id: number,
    price: number,
    currency: string,
    position: ItemPositions,
    imageUrl: string,
    selected: boolean
}