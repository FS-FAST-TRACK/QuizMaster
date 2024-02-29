import Image from 'next/image'

const CheckMarkIcon = ( {width = 100, height = 100}) => {
  return (
    <Image width={width} height={height} src="https://github.com/jaymar921/Public-Repo/blob/main/quizmaster/check-mark-icon-transparent-background.png?raw=true" alt='checkmark' />
  )
}

export default CheckMarkIcon