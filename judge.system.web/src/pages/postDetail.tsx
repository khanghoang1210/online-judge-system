import React from "react";
import Slider from "@/components/Slider/Slider";
import { useRouter } from "next/router";

export default function PostDetail() {
  const router = useRouter();
  const { id } = router.query;

  // Fake data
  const post = {
    id: { id },
    title: `Dynamic Programming`,
    content: `Nội dung chi tiết của bài post ${id}.`,
    difficulty: "Trung bình",
    category: "Thuật toán",
    solution: "Nội dung giải pháp nằm ở đây...",
  };

  const a = `<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>In-Place Array Operations Introduction</title>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <style type="text/css">
        /**
         * No mixins for output here.
         * If you want to expose some mixins,
         * define them to legacy/common/styles/atomic
         */
        /* stylelint-disable */
        .content-base {
            position: relative;
            height: 100%;
        }
        .content-base .content-title-base {
            display: -webkit-box;
            display: -ms-flexbox;
            display: flex;
        }
        .content-base .content-title {
            -webkit-box-flex: 1;
                -ms-flex: 1;
                    flex: 1;
            font-size: 30px;
            font-weight: 600;
            line-height: 38px;
        }
        .content-base .article-actions {
            display: -webkit-box;
            display: -ms-flexbox;
            display: flex;
            -webkit-box-orient: vertical;
            -webkit-box-direction: normal;
                -ms-flex-direction: column;
                    flex-direction: column;
            -webkit-box-align: end;
                -ms-flex-align: end;
                    align-items: flex-end;
        }
        .content-base .article-actions > *:not(:first-child) {
            margin-top: 2px;
        }
        .content-base .article-base {
            background: #fafafa;
            margin-bottom: 120px;
        }
        .content-base .article-base .block-markdown {
            background: #fafafa;
        }
        .content-base .viewer-base {
            margin-bottom: 0;
        }
        .content-base .article-base {
            height: 100%;
            overflow-y: auto;
            -webkit-overflow-scrolling: touch;
            padding: 0 20px;
        }
        .content-base .article-base .article-inner {
            position: relative;
            margin: auto;
            padding: 20px;
            margin-top: 20px;
            margin-bottom: 20px;
            max-width: 840px;
            background: white;
            border-radius: 15px;
        }
        .content-base .article-base .block-markdown {
            background: white;
        }
    </style>
</head>
<body>
    <div class="content-base">
        <div class="article-base">
            <div class="article-inner">
                <div class="content-title-base">
                    <span class="content-title">
                        <span><i title="Article" class="fa fa-font content-icon" aria-hidden="true"></i> &nbsp;</span>
                        In-Place Array Operations Introduction
                    </span>
                    <a href="https://github.com/LeetCode-Feedback/LeetCode-Feedback/issues" target="_blank" rel="noopener noreferrer">Report Issue</a>
                </div>
                <div class="block-markdown">
                    <hr>
                    <blockquote>
                        <p>In programming interviews, the interviewer often expects you to minimise the time and space complexity of your implementation. In-place Array operations help to reduce space complexity, and so are a class of techniques that pretty much everybody encounters regularly in interviews.</p>
                    </blockquote>
                    <p>So, what <em>are</em> in-place array operations?</p>
                    <p>The best way of answering this question is to look at an example.</p>
                    <blockquote>
                        <p>Given an Array of integers, return an Array where every element at an even-indexed position is squared.</p>
                    </blockquote>
                    <pre>
<b>Input:</b> array = [9, -2, -9, 11, 56, -12, -3]
<b>Output:</b> [81, -2, 81, 11, 3136, -12, 9]
<b>Explanation:</b> The numbers at even indexes (0, 2, 4, 6) have been squared, 
whereas the numbers at odd indexes (1, 3, 5) have been left the same.
                    </pre>
                    <p>This problem is hopefully very straightforward. Have a quick think about how you would implement it as an algorithm though, possibly jotting down some code on a piece of paper.</p>
                    <p>Anyway, there are two ways we could approach it. The first is to create a new Array, of the same size as the original. Then, we should copy the odd-indexed elements and square the even-indexed elements, writing them into the new Array.</p>
                    <pre>
                        public int[] squareEven(int[] array, int length) {

                            // Check for edge cases.
                            if (array == null) {
                              return null;
                            }
                          
                            // Create a resultant Array which would hold the result.
                            int result[] = new int[length];
                          
                            // Iterate through the original Array.
                            for (int i = 0; i < length; i++) {
                          
                              // Get the element from slot i of the input Array.
                              int element = array[i];
                          
                              // If the index is an even number, then we need to square element.
                              if (i % 2 == 0) {
                                element *= element;
                              }
                          
                              // Write element into the result Array.
                              result[i] = element;
                            }
                          
                            // Return the result Array.
                            return result;
                          }
                    </pre>
                    <p></p>
                    <center>
                        <img src="https://leetcode.com/explore/learn/card/fun-with-a…ce-operations/Figures/Array_Explore/inplace-1.png" width="600">
                    </center>
                    <br>
                    <p></p>
                    <p>The above approach, although correct, is an <em>inefficient</em> way of solving the problem. This is because it uses <span class="maths katex-rendered"><span class="katex"><span class="katex-mathml"><math><semantics><mrow><mi>O</mi><mo>(</mo><mtext>length</mtext><mo>)</mo></mrow><annotation encoding="application/x-tex">O(\text{length})</annotation></semantics></math></span><span class="katex-html" aria-hidden="true"><span class="base"><span class="strut" style="height: 1em; vertical-align: -0.25em;"></span><span class="mord mathdefault" style="margin-right: 0.02778em;">O</span><span class="mopen">(</span><span class="mord text"><span class="mord">length</span></span><span class="mclose">)</span></span></span></span></span> extra space.</p>
                    <p>Instead, we could iterate over the original input Array itself, overwriting every even-indexed element with its own square. This way, we won't need that extra space. It is this technique of working directly in the input Array, and <em>not</em> creating a new Array, that we call <strong>in-place</strong>. In-place Array operations are a big deal for programming interviews, where there is a big focus on minimising both time and space complexity of algorithms.</p>
                    <p>Here's the in-place implementation for our <code>squareEven(...)</code> function.</p>
                    <pre>
                        public int[] squareEven(int[] array, int length) {

                            // Check for edge cases.
                            if (array == null) {
                              return array;
                            }
                          
                            // Iterate through even elements of the original array.
                            // Notice how we don't need to do *anything* for the odd indexes? :-)
                            for (int i = 0; i < length; i += 2) {
                          
                              // Index is an even number, so we need to square the element
                              // and replace the original value in the Array.
                              array[i] *= array[i];
                          
                            }
                          
                            // We just return the original array. Some problems on leetcode require you
                            // to return it, and other's don't.
                            return array;
                          }
                    </pre>
                    <p>Here's an animation showing the algorithm!</p>
                    <p></p>
                    <center>
                        <p></p>
                        <div>
                            <div>
                                <div class="dia-container__jsK9" style="width: 500px; height: 223px;">
                                    <div class="diaporama__1pV2">
                                        <img alt="Current" class="dia-img__3g12" src="blob:https://leetcode.com/60538095-713a-4c4f-bdf5-2698d6edddcd">
                                    </div>
                                    <div class="initial-play-wrapper__2HUL">
                                        <div class="initial-play__2m2N play-container__2y5J">
                                            <svg viewBox="0 0 24 24" width="1em" height="1em" class="icon__1Md2">
                                                <defs>
                                                    <path id="play-arrow_svg__a" d="M8 5v14l11-7z"></path>
                                                    <mask id="play-arrow_svg__b">
                                                        <use fill-rule="evenodd" xlink:href="#play-arrow_svg__a"></use>
                                                    </mask>
                                                </defs>
                                                <g fill-rule="evenodd">
                                                    <use xlink:href="#play-arrow_svg__a"></use>
                                                    <g fill-rule="nonzero" mask="url(#play-arrow_svg__b)">
                                                        <path d="M0 0h24v24H0z"></path>
                                                    </g>
                                                </g>
                                            </svg>
                                        </div>
                                    </div>
                                    <div class="control-panel__1ogu">
                                        <div class="controls__3i3n">
                                            <svg viewBox="0 0 24 24" width="1em" height="1em" class="icon__1Md2 control-group__3APN">
                                                <path fill-rule="evenodd" d="M15.41 7.41L14 6l-6 6 6 6 1.41-1.41L10.83 12z"></path>
                                            </svg>
                                            <svg viewBox="0 0 24 24" width="1em" height="1em" class="icon__1Md2 toggle-play__3nt0 control-group__3APN">
                                                <defs>
                                                    <path id="play-arrow_svg__a" d="M8 5v14l11-7z"></path>
                                                    <mask id="play-arrow_svg__b">
                                                        <use fill-rule="evenodd" xlink:href="#play-arrow_svg__a"></use>
                                                    </mask>
                                                </defs>
                                                <g fill-rule="evenodd">
                                                    <use xlink:href="#play-arrow_svg__a"></use>
                                                    <g fill-rule="nonzero" mask="url(#play-arrow_svg__b)">
                                                        <path d="M0 0h24v24H0z"></path>
                                                    </g>
                                                </g>
                                            </svg>
                                            <svg viewBox="0 0 24 24" width="1em" height="1em" class="icon__1Md2 control-group__3APN">
                                                <path fill-rule="evenodd" d="M10 6L8.59 7.41 13.17 12l-4.58 4.59L10 18l6-6z"></path>
                                            </svg>
                                        </div>
                                        <div class="frame-counter__mLmP">1 / 8</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </center>
                    <p>An important difference for in-place vs not in-place is that in-place <em>modifies the input Array</em>. This means that other functions <em>can no longer access the original data</em>, because it has been overwritten. We'll talk more about this in a bit.</p>
                    <p><br></p>
                    <hr>
                    <p>We now have a couple of straightforward in-place problems for you to try. Remember, you aren't allowed to create any new Arrays (or any other data structures). If the return type of the question is an Array, then simply return the input Array once you've modified it.</p>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
`

 

  return (
    <div className="container mx-auto  mt-4">
      <Slider title="Featured" />
      <div className="w-3/4 bg-white p-8 shadow-md ml-4">
        <h1 className="text-3xl font-bold text-gray-800">{post.title}</h1>
        <p className="mt-4 text-gray-700">ID hiện ở đây nè :{post.content}</p>
        <div className="mt-6">
        <div dangerouslySetInnerHTML={{__html: a}} />
          {/* <h2 className="text-xl font-semibold text-gray-800">
            Độ khó: {post.difficulty}
          </h2>
          <h2 className="text-xl font-semibold text-gray-800">
            Danh mục: {post.category}
          </h2>
          <h2 className="text-xl font-semibold text-gray-800">Giải pháp:</h2>
          <p className="mt-2 text-gray-700">{post.solution}</p> */}
        </div>
      </div>
    </div>
  );
}
