import Head from 'next/head'
import Image from 'next/image'
import { Inter } from 'next/font/google'
import styles from '@/styles/Home.module.css'
import Topbar from '@/components/Topbar/Topbar'
import Slider from '@/components/Slider/Slider'
import ProblemsTable from '@/components/ProblemsTable/ProblemsTable'

const inter = Inter({ subsets: ['latin'] })

export default function Home() {
  return (
    <>
    <main className='bg-dark-layer-2 min-h-screen'>
      <Topbar/>
			<Slider title='Featured'/>
      <Slider title='Interview'/>
      <Slider title='Learn'/>
    </main>
    </>
  )
}
