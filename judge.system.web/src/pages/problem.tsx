import Head from 'next/head'
import Image from 'next/image'
import { Inter } from 'next/font/google'
import styles from '@/styles/Home.module.css'
import Topbar from '@/components/Topbar/Topbar'
import ProblemsTable from '@/components/ProblemsTable/ProblemsTable'

const inter = Inter({ subsets: ['latin'] })

export default function Home() {
  return (
    <>
    <main className='bg-dark-layer-2 min-h-screen'>
      <Topbar/>
      <h1
					className='text-2xl text-center text-gray-700 dark:text-gray-400 font-medium
					uppercase mt-10 mb-5'
				>
					{/* &ldquo; QUALITY OVER QUANTITY &rdquo; 👇 */}
          Problems
				</h1>
				<div className='relative overflow-x-auto mx-auto px-6 pb-10'>
					{ (
						<div className='max-w-[1200px] mx-auto sm:w-7/12 w-full animate-pulse'>
							{/* {[...Array(10)].map((_, idx) => (
								<LoadingSkeleton key={idx} />
							))} */}
						</div>
					)}
					<table className='text-sm text-left text-gray-500 dark:text-gray-400 sm:w-7/12 w-full max-w-[1200px] mx-auto'>
						{(
							<thead className='text-xs text-gray-700 uppercase dark:text-gray-400 border-b '>
								<tr>
									<th scope='col' className='px-1 py-3 w-0 font-medium'>
										Status
									</th>
									<th scope='col' className='px-6 py-3 w-0 font-medium'>
										Title
									</th>
									<th scope='col' className='px-6 py-3 w-0 font-medium'>
										Difficulty
									</th>

									<th scope='col' className='px-6 py-3 w-0 font-medium'>
										Category
									</th>
									<th scope='col' className='px-6 py-3 w-0 font-medium'>
										Solution
									</th>
								</tr>
							</thead>
						)}
						<ProblemsTable />
					</table>
				</div>
    </main>
    </>
  )
}
	