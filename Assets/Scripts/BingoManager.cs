using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BingoManager : MonoBehaviour
{
	[SerializeField]
	public Text button_text;

	[SerializeField]
	private Sprite logo;//ロゴ画像．見せたくないときに使う

	[SerializeField]
	private List<string> m_name = new List<string>();//名前
	[SerializeField]
	private List<Sprite> m_image = new List<Sprite>();//写真

	//--- UIコンポーネント ---
	private Text name_text;
	private Image image;

	//--- Audio ---
	private AudioSource audio;
	[SerializeField]
	private AudioClip pi;
	[SerializeField]
	private AudioClip picon;

	private int getnumber = 0;
  
	private bool roulette = false;
	private float roulette_time = 0.0f;
	private float roulette_interval = 0.001f;

	private int popnum = 0;

	// Use this for initialization
	void Start()
	{
		name_text = GameObject.Find("MentorName").GetComponent<Text>();
		image = GameObject.Find("MentorImage").GetComponent<Image>();
		audio = GetComponent<AudioSource>();

		//----- 表示 -----
		name_text.text = m_name[getnumber];
		image.sprite = m_image[getnumber];

	}

	// Update is called once per frame
	void Update()
	{  
		if (roulette)
		{
			button_text.text = "ROULETTE!";

			roulette_time += Time.deltaTime;

			if (roulette_time > roulette_interval)
			{
				//----- 連続して同じ画像が出ないようにする仕組み -----
				if (m_name.Count > 1)
				{
					int pastnum = popnum;
					while (popnum == pastnum)
					{
						popnum = Random.Range(0, m_name.Count);
					}
				}
				else
				{
					popnum = 0;
				}

				//----- 音を再生 -----
				audio.clip = pi;
				audio.Play();

				//----- 表示 -----
				name_text.text = m_name[popnum];
				image.sprite = m_image[popnum];


				//----- 徐々に早くなる仕組み -----
				if (roulette_interval < 0.1f)
				{
					roulette_interval += 0.003f;
				}
				else if (roulette_interval < 0.4f && roulette_interval > 0.1f)
				{
					roulette_interval += 0.01f;
				}
				else if (roulette_interval < 0.45f)
				{
					roulette_interval += 0.1f;
					//----- 表示 -----
					audio.clip = picon;
          audio.Play();
          name_text.text = m_name[getnumber];
          image.sprite = m_image[getnumber];
				}
				else
				{
					roulette_interval += 0.05f;
				}


				roulette_time = 0.0f;

        //----- ルーレットの終了 -----
				if (roulette_interval > 0.5f)
				{
					roulette_interval = 0.001f;
					roulette = false;//ルーレットの終了
				}
			}

		}
		else
		{
			button_text.text = "BINGO!";

			if (m_name.Count > 1)
			{
				//----- 表示 -----
				name_text.text = m_name[getnumber];
				image.sprite = m_image[getnumber];
			}
			else
      {
        name_text.text = "おわり！";
				image.sprite = logo;
			}
		}

		if (Input.GetKeyDown(KeyCode.Space))
			BingoButton();

	}

	/* ----------------------------------------------------------------- BingoButton()
   * 「Bingo」ボタンが押された時の挙動
   */
	public void BingoButton()
	{
		if (!roulette)
		{
			if (m_name.Count > 1)
			{
				
        //--- 一旦適当な画像表示 ---
				popnum = Random.Range(0, m_name.Count);
        name_text.text = m_name[popnum];
        image.sprite = m_image[popnum];

				//----- 現在表示中データの削除 -----
        m_name.RemoveAt(getnumber);
        m_image.RemoveAt(getnumber);

        //----- 乱数取得 -----
				getnumber = Random.Range(0, m_name.Count);
				roulette = true;
			}
			else
			{
				name_text.text = "おわり！";
				image.sprite = logo;
			}
		}
	}

}
