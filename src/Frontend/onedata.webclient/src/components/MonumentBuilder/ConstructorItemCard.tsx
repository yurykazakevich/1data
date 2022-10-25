import { SyntheticEvent, useEffect, useState } from 'react';
import { Button, Card } from 'react-bootstrap';
import { useImageConverter } from '../../hooks/imageConverter';
import { IListItemResponse, ISelectedCItem } from '../../models/MonumentBuilder';

function ConstructorItemCard(props: {
    item: IListItemResponse,
    showItemDetails: (itemId: number) => void,
    handleItemSelected: (selectedItem: ISelectedCItem) => void
}) {
    var imageUrl: string = ''

    const cItem = props.item
    const makeImage = useImageConverter().base64toBlob
    const [selected, setSelected] = useState(false)

    useEffect(() => {
        const image: Blob = makeImage(cItem.image)
        imageUrl = URL.createObjectURL(image)
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
                imageUrl: imageUrl,
                selected: !selected
            })
        }

        setSelected((prevSelected) => !prevSelected)
    }

    return (
        <Card className={"item-card " + selected ? "shadow-sm" : "shadow-none"} onClick={ selectItem }>
          <Card.Img variant="top" src={imageUrl} onLoad={ imageOnLoad } />
          <Card.Body>
              <Card.Text>
                  <div>{props.item.name}</div>
                  <div>{props.item.size}</div>
                  <div className="fw-bold">Цена:&npsp;{props.item.price}&npsp;{props.item.currency}</div>
              </Card.Text>
          </Card.Body>
          <Card.Footer>
              <Button variant="link" className="text-muted" onClick={() => { props.showItemDetails(props.item.id) }}>Подробнее</Button>
          </Card.Footer>
      </Card>
  );
}

export default ConstructorItemCard;