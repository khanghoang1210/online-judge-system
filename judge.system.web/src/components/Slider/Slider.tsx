import React, { useEffect, useState } from "react";
import Slider from "react-slick";
import Image from "next/image";
import Link from "next/link";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
import { SampleNextArrow, SamplePrevArrow } from "./Arrow";

const API_URL = "http://localhost:5107/api/Posts/";

export type Post = {
  id: number;
  title: string;
  introduction: string;
  content: string;
  image: string;
};


interface CustomSliderProps {
  title: string;
}

const CustomSlider: React.FC<CustomSliderProps> = ({ title }) => {
  const [postList, setPostList] = useState<Post[]>([]);
  useEffect(() => {
    fetch(API_URL, {
      method: "GET",
      mode: "cors",
      headers: {
        Accept: "application/json, text/plain",
        "Content-Type": "application/json;charset=UTF-8",
      },
    })
      .then((res) => {
        if (!res.ok) {
          throw new Error("Network response was not ok");
        }
        return res.json();
      })
      .then((data) => {
        if (data && data.data && Array.isArray(data.data)) {
          setPostList(data.data);

          // Update problemMap directly with the fetched data
        } else {
          throw new Error("Invalid data format in the response");
        }
      })
      .catch((err) => console.error("Error fetching problems:", err));
  }, []);
  const settings = {
    dots: true,
    infinite: true,
    speed: 500,
    slidesToShow: 5,
    slidesToScroll: 1,
    nextArrow: <SampleNextArrow />,
    prevArrow: <SamplePrevArrow />,
    responsive: [
      {
        breakpoint: 1024,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 1,
          infinite: true,
          dots: true,
        },
      },
      {
        breakpoint: 600,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1,
        },
      },
    ],
  };
  return (
    <>
      <div className="slider-container px-2 py-2">
        <h2 className="text-3xl font-bold mb-4 text-white">{title}</h2>
  
        <div className="flex flex-wrap -mx-2"> {/* Adjusted container for horizontal wrapping */}
          {postList.map((item, index) => (
            <div key={index} className="w-full sm:w-1/3 px-5 mb-5"> {/* Set width to one-third and added margins */}
              <Link href={`/posts/${item.id}`}>
                <div
                  className="text-3xl bg-white rounded-lg shadow-lg overflow-hidden m-0.5"
                  style={{
                    height: "200px",
                    position: "relative",
                  }}
                >
                  <div
                    style={{
                      position: "relative",
                      height: "100%",
                      width: "100%",
                    }}
                  >
                    <Image
                      src={item.image}
                      alt={item.title}
                      layout="fill"
                      objectFit="cover"
                      objectPosition="center"
                    />
                  </div>
                  <div className="p-4 text-black">
                    <h3
                      className="text-2xl text-white font-bold mb-2"
                      style={{
                        position: "absolute",
                        top: "24px",
                        left: "20px",
                        right: "20px",
                      }}
                    >
                      {item.title}
                    </h3>
                    <div className="flex justify-between text-sm">
                      <span>Chapters</span>
                      <span>Items</span>
                      <span>%</span>
                    </div>
                  </div>
                </div>
              </Link>
            </div>
          ))}
        </div>
      </div>
    </>
  );
  
  
  
};

export default CustomSlider;
