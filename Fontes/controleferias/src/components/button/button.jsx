export default function Button(props) {
    return (
        <button onClick={e => props.onclick()}>{props.icon}<span>{props.span}</span></button>
    )
}