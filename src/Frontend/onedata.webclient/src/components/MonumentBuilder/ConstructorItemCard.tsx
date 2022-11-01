import { SyntheticEvent, useEffect, useState } from 'react';
import { Button, Card } from 'react-bootstrap';
import { useImageConverter } from '../../hooks/imageConverter';
import { IListItemResponse, ISelectedCItem } from '../../models/MonumentBuilder';

function ConstructorItemCard(props: {
    item: IListItemResponse,
    selectedItemId: number,
    showItemDetails: (itemId: number) => void,
    handleItemSelected: (selectedItem: ISelectedCItem) => void
}) {
    const [imageUrl, setImageUrl] = useState('')
    const cItem = props.item
    const makeImage = useImageConverter().base64toBlob

    useEffect(() => {
        const image: Blob = makeImage(cItem.image)
        setImageUrl(URL.createObjectURL(image))
    }, [])

    function imageOnLoad(e: SyntheticEvent<HTMLImageElement> | undefined) {
        if (e?.currentTarget.currentSrc) {
            console.debug("ONLOAD: " + e?.currentTarget.currentSrc)
            URL.revokeObjectURL(e.currentTarget.currentSrc)
        }
    }

    function selectItem() {
        if (props.handleItemSelected) {
            props.handleItemSelected({
                id: cItem.id,
                price: cItem.price,
                currency: cItem.currency,
                position: cItem.position,
                imageUrl: imageUrl
            })
        }
    }

    return (
        <Card className={"item-card " + (props.selectedItemId === props.item.id ? 'item-card-selected' : "")} onClick={selectItem}>
          <Card.Img variant="top" src={imageUrl} onLoad={ imageOnLoad } />
          <Card.Body className="p-2">
              <Card.Text>
                  <div>{props.item.name}</div>
                  <div>{props.item.size}</div>
                  <div className="fw-bold text-nowrap price-container">Цена: {props.item.price} {props.item.currency}</div>
              </Card.Text>
          </Card.Body>
            <Card.Footer className="text-center p-1">
              <Button variant="link" className="text-muted p-0" onClick={() => { props.showItemDetails(props.item.id) }}>Подробнее</Button>
          </Card.Footer>
      </Card>
  );
}

export default ConstructorItemCard;